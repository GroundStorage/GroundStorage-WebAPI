using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ground_Storage_WebAPI.Models
{
    public class ActionOutput
    {
        public string Status { get; set; }
    }

    public class ActionOutputSuccess<T> : ActionOutput
    {
        public ActionOutputSuccess()
        {
            this.Status = "success";
            this.Meta = new ActionMeta();
        }
        public T Data { get; set; }
        public ActionMeta Meta;
    }

    public class ActionOutputError<T> : ActionOutput
    {
        public ActionOutputError()
        {
            this.Status = "error";
        }
        public T Error { get; set; }
    }

    public class ActionMeta
    {
        public string ExecutedQuery { get; set; }
    }
}