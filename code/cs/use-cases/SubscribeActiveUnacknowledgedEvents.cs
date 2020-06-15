var isActive = new OpcSimpleAttributeOperand(
        OpcEventTypes.AlarmCondition,
        "ActiveState", "Id");
var isAcked = new OpcSimpleAttributeOperand(
        OpcEventTypes.AcknowledgeableCondition,
        "AckedState", "Id");

var filter = OpcFilter.Using(client)
    .FromEvents(OpcEventTypes.AlarmCondition)
    .Where(OpcFilterOperand.OfType(OpcEventTypes.AlarmCondition)
        & isAcked == false
        & isActive == true)
    .Select();

var subscription = client.SubscribeEvent("ns=2;s=Machine", filter, (sender, e) => {
    if (e.Event is OpcAlarmCondition alarm) {
        Console.WriteLine(
                "{0}: {1}, IsActive = {2}, IsAcked = {3} ({4})",
                alarm.GetType().Name,
                alarm.Message,
                alarm.IsActive,
                alarm.IsAcked,
                alarm.Severity);
    }
});
