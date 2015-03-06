using BirthdayGreetings.Core;
using Newtonsoft.Json;
using paramore.brighter.commandprocessor;

namespace BirthdayGreetings.Main
{
    internal class SendGreetingCommandMessageMapper : IAmAMessageMapper<SendGreetingCommand>
    {
        public Message MapToMessage(SendGreetingCommand request)
        {
            var header = new MessageHeader(messageId: request.Id, topic: "Send_greeting", messageType: MessageType.MT_COMMAND);
            var body = new MessageBody(JsonConvert.SerializeObject(request));
            return new Message(header, body);
        }

        public SendGreetingCommand MapToRequest(Message message)
        {
            return JsonConvert.DeserializeObject<SendGreetingCommand>(message.Body.Value);
        }
    }
}