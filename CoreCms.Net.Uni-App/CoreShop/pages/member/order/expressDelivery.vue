<template>
    <view class="wrap">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar title="物流信息"></u-navbar>

        <u-time-line v-if="isExpress">
            <u-time-line-item v-for="(item, index) in express" :key="index">
                <template v-slot:content>
                    <view>
                        <view class="u-order-desc">{{ item.context }}</view>
                        <view class="u-order-time">{{ item.time }}</view>
                    </view>
                </template>
            </u-time-line-item>
        </u-time-line>
    </view>
</template>

<script>
    export default {
        data() {
            return {
                add: '', // 收货地址
                express: {}, // 快递物流信息
            }
        },
        onLoad(options) {
            let params = options.params
            let arr = decodeURIComponent(params).split('&')
            let code, no, mobile
            for (var i = 0; i < arr.length; i++) {
                let key = arr[i].split("=")[0]
                if (key == 'code') {
                    code = arr[i].split("=")[1]
                }
                if (key == 'no') {
                    no = arr[i].split("=")[1]
                }
                if (key == 'add') {
                    this.add = arr[i].split('=')[1]
                }
                if (key == 'mobile') {
                    mobile = arr[i].split('=')[1]
                }
            }

            if (!code || !no) {
                this.$refs.uToast.show({ title: '缺少物流查询参数', type: 'error', back: true });
            }
            this.expressInfo(code, no, mobile)
        },
        computed: {
            isExpress() {
                return Object.keys(this.express).length ? true : false
            }
        },
        methods: {
            expressInfo(code, no, mobile) {
                let data = {
                    code: code,
                    no: no,
                    mobile: mobile,
                }
                this.$u.api.logistics(data).then(res => {
                    if (res.status) {
                        let _info = res.data.data
                        this.express = _info
                    } else {
                        this.$u.toast(res.msg)
                    }
                })
            }
        }
    }
</script>

<style lang="scss" scoped>
    .wrap { padding: 24rpx 24rpx 24rpx 40rpx; }
    .u-node { width: 44rpx; height: 44rpx; border-radius: 100rpx; display: flex; justify-content: center; align-items: center; background: #d0d0d0; }
    .u-order-title { color: #333333; font-weight: bold; font-size: 32rpx; }
        .u-order-title.unacive { color: rgb(150, 150, 150); }
    .u-order-desc { color: rgb(150, 150, 150); font-size: 28rpx; margin-bottom: 6rpx; }
    .u-order-time { color: rgb(200, 200, 200); font-size: 26rpx; }
    .tel { color: $u-type-primary; }
</style>
