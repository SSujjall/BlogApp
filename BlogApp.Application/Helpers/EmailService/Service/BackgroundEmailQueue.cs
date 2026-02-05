using BlogApp.Application.Helpers.EmailService.Model;
using System.Threading.Channels;

namespace BlogApp.Application.Helpers.EmailService.Service
{
    public interface IBackgroundEmailQueue
    {
        void QueueEmail(EmailMessage message);
        ChannelReader<EmailMessage> Reader { get; }
    }

    public class BackgroundEmailQueue : IBackgroundEmailQueue
    {
        private readonly Channel<EmailMessage> _queue;

        public BackgroundEmailQueue()
        {
            _queue = Channel.CreateUnbounded<EmailMessage>();
        }
        public void QueueEmail(EmailMessage message)
        {
            _queue.Writer.TryWrite(message);
        }

        public ChannelReader<EmailMessage> Reader => _queue.Reader;
    }
}
