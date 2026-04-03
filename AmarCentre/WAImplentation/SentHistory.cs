using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAImplementation.Library
{
    public class SentHistory
    {
        public Guid MessageId { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string SenderPhoneNumberID { get; set; }
        public string SenderPhoneNumber { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public int MediaTypeId { get; set; }
        public string MessageContent { get; set; }
        public DateTimeOffset MessageSentDate { get; set; }
        public bool IsSentSuccessfully { get; set; }
        public string ResponseText { get; set; } //From Meta API 
        public string MediaTypeName { get; set; }
        public LifecycleStatus LifecycleStatus { get; set; }
        public string CurrentStatus { get; set; }
        public DateTimeOffset LastUpdatedDateTime { get; set; }
    }

    public struct LifecycleStatus
    {
        public Guid MessageId { get; set; }
        public long Timestamp { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public bool HasSent { get; set; }
        public DateTimeOffset SentDateTime { get; set; }
        public bool HasDelivered { get; set; }
        public DateTimeOffset DeliveredDateTime { get; set; }
        public bool HasRead { get; set; }
        public DateTimeOffset ReadDateTime { get; set; }
        public bool HasFailed { get; set; }
        public DateTimeOffset FailedDateTime { get; set; }
        public bool HasDeleted { get; set; }
        public DateTimeOffset DeleteDateTime { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorTitle { get; set; }
        public DateTimeOffset LastUpdatedDateTime { get; set; }
    }
    public class SummaryCard
    {
        public string Label { get; set; }
        public int Value { get; set; }
        public string Trend { get; set; }
        public string TrendClass { get; set; }
    }

}
