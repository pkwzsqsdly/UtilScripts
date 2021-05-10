public class AttInfoCreator
{
    public static AttInfo Create(IAttInfoWrapper wrapper)
    {
        var attList = new AttInfo();
            
        wrapper?.Handler(attList);
            
        return attList;
    }
}