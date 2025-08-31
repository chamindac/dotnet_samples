using System;

namespace di_sample.domain.core.Models
{
    public abstract class BaseModel
    {
        public required string Id { get; set; }

        public DateTime CreatedTimeUtc { get; set; }
    }
}
