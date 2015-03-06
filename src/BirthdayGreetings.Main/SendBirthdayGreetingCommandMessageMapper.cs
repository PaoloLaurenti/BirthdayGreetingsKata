using BirthdayGreetings.Core;
using Newtonsoft.Json;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Main
{
    internal class SendBirthdayGreetingCommandMessageMapper : IAmAMessageMapper<SendBirthdayGreetingsCommand>
    {
        public Message MapToMessage(SendBirthdayGreetingsCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Send_birthday_greeting", messageType: MessageType.MT_COMMAND);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            return new Message(header, body);
        }

        public SendBirthdayGreetingsCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<SendBirthdayGreetingsCommand>(message.Body.Value);            
        }
    }
}