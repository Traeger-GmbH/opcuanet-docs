private delegate void CalculateDelegate(
        int a,
        int b,
        ref int c,
        out int additionResult,
        out int differenceResult);

private void Calculate(
        [OpcArgument("a", Description = "The left operand.")]
        int a,
        [OpcArgument("b", Description = "The right operand.")]
        int b,
        [OpcArgument("c", Description = "Factor")]
        ref int c,
        [OpcArgument("add", Description = "The result of addition.")]
        out int additionResult,
        [OpcArgument("diff", Description = "The result of difference.")]
        out int differenceResult)
{
    additionResult = (a + b) * c;
    differenceResult = (a - b) * c;
}

this.methodNode = new OpcMethodNode(
        this.folderNode,
        nameof(Calculate),
        new CalculateDelegate(this.Calculate));
