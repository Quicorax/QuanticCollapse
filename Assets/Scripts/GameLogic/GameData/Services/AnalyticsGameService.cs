using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsGameService : IService
{
    public async Task Initialize()
    {
        try
        {
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError("Error asking for analytics permissions " + e.Message);
        }
    }

    public void SendEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        parameters ??= new Dictionary<string, object>();
        AnalyticsService.Instance.CustomData(eventName, parameters);
    }

    public void Clear() { }
}