using LiteDB;

namespace PCLActivitySet.Db
{
    public abstract class AbstractLiteDbObject
    {
    }

    public abstract class AbstractLiteDbValue : AbstractLiteDbObject
    {
    }

    public abstract class AbstractLiteDbEntity : AbstractLiteDbObject
    {
        public ObjectId Id { get; set; }
    }
}
