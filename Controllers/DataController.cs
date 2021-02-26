using System;
using System.Collections.Generic;
using System.Linq;
using Ground_Storage_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ground_Storage_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public DataController(
            AppDbContext dbContext
        )
        {
            this._dbContext = dbContext;
        }

        [HttpPut]
        public ActionResult<ActionOutput> Upsert(Record_Upsert record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ActionOutputError<ModelStateDictionary> { Error = ModelState });
            }

            try
            {
                bool editMode = record.Id.HasValue;
                var recordKeys = record.Keys.Split(',');
                Record recordInDB = null;
                if (!editMode) // Inserting
                {
                    DateTime createdAt = DateTime.Now;
                    EntityEntry<Record> recordEntry = _dbContext.Records.Add(new Record()
                    {
                        Entity = record.Entity,
                        Rev = Guid.NewGuid(),
                        OfflineId = record.OfflineId,
                        CreatedAt = createdAt,
                        UpdatedAt = createdAt,
                        CreatorId = record.CreatorId,
                        Keys = record.Keys,
                        Body = record.Body,
                    });
                    _dbContext.SaveChanges();
                    recordInDB = recordEntry.Entity;
                }
                else // Updating
                {
                    recordInDB = _dbContext.Records.Find(record.Id);
                    // TODO: check if record found

                    DateTime updatedAt = DateTime.Now;

                    _dbContext.Conflicts.RemoveRange(_dbContext.Conflicts.Where(conflict => !record.Conflicts.Contains(conflict.Rev)));
                    if (record.Rev != recordInDB.Rev)
                    {
                        recordInDB.Conflicts.Add(new Conflict
                        {
                            Entity = recordInDB.Entity,
                            RecordId = recordInDB.Id,
                            Rev = recordInDB.Rev,
                            OfflineId = recordInDB.OfflineId,
                            CreatedAt = recordInDB.CreatedAt,
                            UpdatedAt = recordInDB.UpdatedAt,
                            CreatorId = recordInDB.CreatorId,
                            Keys = recordInDB.Keys,
                            Body = recordInDB.Body,
                        });
                    }

                    recordInDB.Rev = Guid.NewGuid();
                    recordInDB.UpdatedAt = updatedAt;
                    recordInDB.Keys = record.Keys;
                    recordInDB.Body = record.Body;

                    _dbContext.Keys.RemoveRange(_dbContext.Keys.Where(key => key.RecordId == recordInDB.Id && recordKeys.Contains(key.Name)));
                }

                foreach (string key in recordKeys.Where(recordKey => !_dbContext.Keys.Any(key => key.Name == recordKey)))
                {
                    _dbContext.Keys.Add(new Key()
                    {
                        RecordId = recordInDB.Id,
                        Name = key
                    });
                }
                _dbContext.SaveChanges();

                return Ok(new ActionOutputSuccess<Record_Get>
                {
                    Data = new Record_Get
                    {
                        Entity = recordInDB.Entity,
                        Id = recordInDB.Id,
                        Rev = recordInDB.Rev,
                        OfflineId = recordInDB.OfflineId,
                        CreatedAt = recordInDB.CreatedAt,
                        UpdatedAt = recordInDB.UpdatedAt,
                        CreatorId = recordInDB.CreatorId,
                        Keys = recordInDB.Keys,
                        Body = recordInDB.Body,
                        Conflicts = recordInDB.Conflicts.Select(conflict => conflict.Rev).ToArray(),
                    }
                });
            }
            catch (System.Exception error)
            {
                return BadRequest(new ActionOutputError<Exception>{ Error = error});
            }
        }

        [HttpPost]
        public ActionResult<ActionOutput> Get(Record_Search search, [FromQuery] bool metaExecutedQuery = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ActionOutputError<ModelStateDictionary> { Error = ModelState });
            }

            try
            {
                IQueryable<Record> searchedByKeys = _dbContext.Records;
                if (!string.IsNullOrEmpty(search.KeySearch))
                {
                    IEnumerable<string> searchKeys = search.KeySearch.Split(',').Select(key => key.Trim());

                    var queries = new List<IQueryable<Key>>();
                    foreach (var searchKey in searchKeys)
                    {
                        queries.Add(_dbContext.Keys.Where(key => EF.Functions.Like(key.Name, searchKey)));
                    }

                    var finalQuery = queries[0];
                    for (var i = 1; i < queries.Count; i++)
                    {
                        if (search.KeySearchType == KeySearchType.Or)
                        {
                            finalQuery = finalQuery.Union(queries[i]);
                        }
                        else // search.KeySearchType == KeySearchType.And
                        {
                            finalQuery = finalQuery.Join(queries[i], key => key.RecordId, key => key.RecordId, (keyLeft, keyRight) => new Key { RecordId = keyLeft.RecordId });
                        }

                    }
                    var searchedIds = finalQuery
                        .GroupBy(key => key.RecordId);
                    // .Where(group => (search.KeySearchType == KeySearchType.And && group.Count() == searchKeys.Count()) || (search.KeySearchType == KeySearchType.Or && group.Count() > 0));

                    searchedByKeys = searchedByKeys
                        .Where(record => searchedIds.Any(searchedId => searchedId.Key == record.Id));
                }

                var result = searchedByKeys
                    .Where(record =>
                        (string.IsNullOrEmpty(search.Entity) || record.Entity == search.Entity)
                        && (search.Id == null || record.Id == search.Id.Value)
                        && (search.Rev == null || record.Rev == search.Rev.Value)
                        && (search.CreatedAt == null || record.CreatedAt == search.CreatedAt.Value)
                        && (search.UpdatedAt == null || record.UpdatedAt == search.UpdatedAt.Value)
                        && (search.CreatorId == null || record.CreatorId == search.CreatorId.Value)
                    )
                    .Select(record => new Record_Get()
                    {
                        Entity = record.Entity,
                        Id = record.Id,
                        Rev = record.Rev,
                        CreatedAt = record.CreatedAt,
                        UpdatedAt = record.UpdatedAt,
                        CreatorId = record.CreatorId,
                        Keys = record.Keys,
                        Body = record.Body,
                        Conflicts = record.Conflicts.Select(conflict => conflict.Rev).ToArray(),
                    });

                var resultToReturn = new ActionOutputSuccess<IEnumerable<Record_Get>> { Data = result };
                if (metaExecutedQuery)
                {
                    resultToReturn.Meta.ExecutedQuery = result.ToQueryString();
                }

                return Ok(resultToReturn);
            }
            catch (System.Exception error)
            {
                return BadRequest(new ActionOutputError<System.Exception> { Error = error });
            }
        }
    }
}