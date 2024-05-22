using Chattland.DataTransferContract.ChatContracts;

namespace Chattland.Api.DataAccess.Entities;

public class MessageSenderDocument : IMessageSender
{
	public string Name { get; set; }
}