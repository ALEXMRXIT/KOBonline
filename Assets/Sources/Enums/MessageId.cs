namespace Assets.Sources.Enums
{
    public enum MessageId : byte
    {
        MessageSimpleMessage = 0x00,
        MessageLoginIsEmpty = 0x01,
        MessagePasswordIsEmpty = 0x02,
        MessageOperationFail = 0x03,
        ReasonUserOrPassWrong = 0x04,
        MessageCharacterCreate = 0x05,
        MessageCharacterNameIsEmpty = 0x06,
        MessageNameExists = 0x07,
        MessageInvalidNamePattern = 0x08,
        MessageBlockCreateCharacter = 0x09,
        MessageMaxCharacter = 0x0A,
        MessageIncorrectName = 0x0B,
        MessageGameRun = 0x0C,
        MessageVersionClientFail = 0x0D
    }
}