namespace Assets.Sources.Enums
{
    public enum MessageId : byte
    {
        MessageSimpleMessage = 0x00,
        MessageLoginIsEmpty = 0x01,
        MessagePasswordIsEmpty = 0x02,
        MessageOperationFail = 0x03,
        ReasonUserOrPassWrong = 0x04,
        MessageCharacterCreate = 0x05
    }
}