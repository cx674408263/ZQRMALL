<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="支付结果"></u-navbar>

        <!--状态图标-->
        <view class="coreshop-status-view">
            <image class="status-img" src="/static/images/payments/pay.png" mode="widthFix" />
            <view class="bg-red status-bg-view">
                <text class="cuIcon-check icon" v-if="status && paymentInfo.status === 2"></text>
                <text class="cuIcon-roundclosefill icon" v-else-if="status && paymentInfo.status === 1"></text>
            </view>
        </view>
        <!--状态标题-->
        <view class="text-bold text-black text-xl text-center" v-if="status && paymentInfo.status === 2">支付成功</view>
        <view class="text-bold text-black text-xl text-center" v-else-if="status && paymentInfo.status === 1">支付失败</view>


        <!--支付金额-->
        <view class="text-bold text-black text-xl text-center chsop-pay-result-price">￥{{ paymentInfo.money || '' }}</view>

        <!--状态说明-->
        <view class="text-gray text-sm text-center coreshop-padding" v-if="status && paymentInfo.status === 2"> 平台已收到您的钱款，已通知卖家发货。平台会及时通知您的交易状态，请您关注。 </view>
        <view class="text-gray text-sm text-center coreshop-padding" v-else-if="status && paymentInfo.status === 1"> 因为某些问题导致支付失败，请查看详情了解具体问题 </view>

        <!--按钮-->
        <view class="coreshop-btn-view">
            <button class="cu-btn radius line-red" @click="orderDetail()">查看详情</button>
        </view>


        <!-- 登录提示 -->
		<corecms-login-modal></corecms-login-modal>
    </view>
</template>
<script>
    export default {
        data() {
            return {
                paymentId: 0,
                paymentInfo: {}, // 支付单详情
                orderId: 0,
                status: false
            };
        },
        onLoad(options) {
            if (options.id) {
                this.paymentId = options.id;
            }
            if (options.order_id) {
                this.orderId = options.order_id;
            }
        },
        mounted() {
            this.getPaymentInfo();
        },
        methods: {
            getPaymentInfo() {
                if (!this.paymentId) {
                    this.status = true;
                    this.paymentInfo.money = '0.00';
                    this.paymentInfo.status = 2;
                    this.paymentInfo.type = 1;
                    return;
                }
                let data = {
                    id: this.paymentId
                };
                this.$u.api.paymentInfo(data).then(res => {
                    if (res.status) {
                        let info = res.data;
                        if (info.payment_code === 'alipay') {
                            info.payment_name = '支付宝支付';
                        } else if (info.payment_code === 'wechatpay') {
                            info.payment_name = '微信支付';
                        } else if (info.payment_code === 'balancepay') {
                            info.payment_name = '余额支付';
                        }
                        // 获取订单号
                        if (info.rel.length) {
                            for (let i = 0; i < info.rel.length; i++) {
                                if (info.rel[i].sourceId) {
                                    this.orderId = info.rel[i].sourceId;
                                    break;
                                }
                            }
                        }
                        this.status = true;
                        this.paymentInfo = info;
                    } else {
                        this.$u.toast(res.msg);
                    }
                });
            },
            orderDetail() {
                if (this.orderId && this.paymentInfo.type === 1) {
                    this.$u.route({ type: 'redirectTo', url: '/pages/member/order/orderdetail?orderId=' + this.orderId });
                } else if (this.paymentInfo.type === 2) {
                    this.$u.route({ type: 'redirectTo', url: '/pages/member/balance/details' });
                } else if (this.paymentInfo.type === 3 || this.paymentInfo.type === 4) {
                    this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' });
                } else if (this.paymentInfo.type === 5) {
                    this.$u.route({ type: 'redirectTo', url: '/pages/member/serviceOrder/index' });
                }
            }
        }
    };
</script>

<style lang="scss">
    @import '../../../static/style/payStatus.scss';
</style>