// DOC
OpcValue value = client.ReadNode("ns=2;s=Machine/Job/Speed");

// DOC
if (value.Status.IsGood) {
    // Your code to operate on the value.
}

// DOC
int intValue = (int)value.Value;

// DOC
int[] intValues = (int[])value.Value;

// DOC
OpcStatus status = client.WriteNode("ns=2;s=Machine/Job/Speed", 1200);

// DOC
int[] values = new int[3] { 1200, 1350, 1780 };
OpcStatus status = client.WriteNode("ns=2;s=Machine/Job/Speeds", values);

// DOC
if (!status.IsGood) {
    // Your code to handle a failed write operation.
}

// DOC
using (var client = new OpcClient("opc.tcp://localhost:4840")) {
    client.Connect();
    OpcValue arrayValue = client.ReadNode("ns=2;s=Machine/Job/Speeds");
 
    if (arrayValue.Status.IsGood) {
        int[] intArrayValue = (int[])arrayValue.Value;
 
        intArrayValue[2] = 100;
        intArrayValue[4] = 200;
        intArrayValue[9] = 300;
 
        OpcStatus status = client.WriteNode("ns=2;s=Machine/Job/Speeds", intArrayValue);
 
        if (!status.IsGood)
            Console.WriteLine("Failed to write array value!");
    }
}
