using System.Diagnostics;

namespace Microservice2;

public class AppActivity
{
    public static ActivitySource Source = new ActivitySource("Microservice2.API.App", "1.0.0");

     static AppActivity()
    {
        
        ActivitySource.AddActivityListener(new ActivityListener()
        {
            ShouldListenTo = (source) => true,
            Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
            ActivityStarted = activity =>
            {
                Console.WriteLine("---------Activity Start----------------");
                
                WriteActivity("Started", activity);
            },
            ActivityStopped = activity =>
            {
                Console.WriteLine("---------Activity End------------------");
                WriteActivity("Stopped", activity);
            }
        });    
    }

     private static void WriteActivity(string activityType, Activity activity)
     {
         Console.WriteLine($"User Id Baggage :{activity.GetBaggageItem("userId")}");
         Console.WriteLine(activityType + ": " + activity.OperationName);
         Console.WriteLine("Activity Kind: " + activity.Kind);
         Console.WriteLine("Activity Id: " + activity.Id);
         Console.WriteLine("Parent Id: " + activity.ParentId);
         Console.WriteLine("Duration: " + activity.Duration);
         
     }
    
    
}
