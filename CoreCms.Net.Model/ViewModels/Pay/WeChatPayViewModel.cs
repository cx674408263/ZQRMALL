﻿/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.ViewModels.Pay
{
    public class WeChatPayMicroPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "auth_code")]
        public string AuthCode { get; set; }
    }

    public class WeChatPayPubPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }

        [Required] [Display(Name = "openid")] public string OpenId { get; set; }
    }

    public class WeChatPayQrCodePayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required] [Display(Name = "body")] public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WeChatPayAppPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required] [Display(Name = "body")] public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WeChatPayH5PayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required] [Display(Name = "body")] public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WeChatPayLiteAppPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required] [Display(Name = "body")] public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }

        [Required] [Display(Name = "openid")] public string OpenId { get; set; }
    }

    public class WeChatPayOrderQueryViewModel
    {
        [Display(Name = "transaction_id")] public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")] public string OutTradeNo { get; set; }
    }

    public class WeChatPayReverseViewModel
    {
        [Display(Name = "transaction_id")] public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")] public string OutTradeNo { get; set; }
    }

    public class WeChatPayCloseOrderViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
    }

    public class WeChatPayRefundViewModel
    {
        [Required]
        [Display(Name = "out_refund_no")]
        public string OutRefundNo { get; set; }

        [Display(Name = "transaction_id")] public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")] public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "refund_fee")]
        public int RefundFee { get; set; }

        [Display(Name = "refund_desc")] public string RefundDesc { get; set; }

        [Display(Name = "notify_url")] public string NotifyUrl { get; set; }
    }

    public class WeChatPayRefundQueryViewModel
    {
        [Display(Name = "refund_id")] public string RefundId { get; set; }

        [Display(Name = "out_refund_no")] public string OutRefundNo { get; set; }

        [Display(Name = "transaction_id")] public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")] public string OutTradeNo { get; set; }
    }

    public class WeChatPayDownloadBillViewModel
    {
        [Required]
        [Display(Name = "bill_date")]
        public string BillDate { get; set; }

        [Required]
        [Display(Name = "bill_type")]
        public string BillType { get; set; }

        [Display(Name = "tar_type")] public string TarType { get; set; }
    }

    public class WeChatPayDownloadFundFlowViewModel
    {
        [Required]
        [Display(Name = "bill_date")]
        public string BillDate { get; set; }

        [Required]
        [Display(Name = "account_type")]
        public string AccountType { get; set; }

        [Display(Name = "tar_type")] public string TarType { get; set; }
    }

    public class WeChatPayTransfersViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }

        [Required] [Display(Name = "openid")] public string OpenId { get; set; }

        [Required]
        [Display(Name = "check_name")]
        public string CheckName { get; set; }

        [Display(Name = "re_user_name")] public string ReUserName { get; set; }

        [Required] [Display(Name = "amount")] public int Amount { get; set; }

        [Required] [Display(Name = "desc")] public string Desc { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpBillCreateIp { get; set; }
    }

    public class WeChatPayGetTransferInfoViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }
    }

    public class WeChatPayPayBankViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }

        [Required]
        [Display(Name = "enc_bank_no")]
        public string EncBankNo { get; set; }

        [Required]
        [Display(Name = "enc_true_name")]
        public string EncTrueName { get; set; }

        [Required]
        [Display(Name = "bank_code")]
        public string BankCode { get; set; }

        [Required] [Display(Name = "amount")] public int Amount { get; set; }

        [Display(Name = "desc")] public string Desc { get; set; }
    }

    public class WeChatPayQueryBankViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }
    }
}