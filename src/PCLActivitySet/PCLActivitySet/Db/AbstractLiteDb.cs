using LiteDB;

namespace PCLActivitySet.Db
{
    public abstract class AbstractLiteDb
    {
    }

    public abstract class AbstractLiteDbValue : AbstractLiteDb
    {
    }

    public abstract class AbstractLiteDbEntity : AbstractLiteDb
    {
        public ObjectId Id { get; set; }
    }
}
