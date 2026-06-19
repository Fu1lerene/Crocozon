namespace Crocozon.Library.ValueObjects.Helpers;

public static class IdConverter
{
    public static Guid ToGuid(this long id)
    {
        var bytes = new byte[16];
        BitConverter.GetBytes(id).CopyTo(bytes, 0); 
        
        return new Guid(bytes);
    }

    public static long ToLong(this Guid guid)
    {
        var bytes = guid.ToByteArray();

        return BitConverter.ToInt64(bytes, 0); 
    }
}