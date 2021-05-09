public class CustomAttListCreator
{
    public static CustomAttList Create(ICustomAttListWrapper wrapper)
    {
        var attList = new CustomAttList();
            
        wrapper?.Handler(attList);
            
        return attList;
    }
}