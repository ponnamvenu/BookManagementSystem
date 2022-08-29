namespace HelloWeb.services
{
    public class TimeUtils
    {
        public string TimePrefix
        {
            get{
               
                var hrs = DateTime.Now.Hour;
                if (hrs < 12)
                    return "Morning";
                else if (hrs < 18)
                    return "Afternoon";
                else
                    return "Evening";                   

            }
        }

        
    }
}