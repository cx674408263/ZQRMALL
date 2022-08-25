import Vue from 'vue'
import App from './App'

import * as Upload from '@/common/utils/uploadHelper.js'

import * as Common from '@/common/utils/commonHelper.js'
import * as Db from '@/common/utils/dbHelper.js'
import * as Config from '@/common/setting/constVarsHelper.js'
import store from '@/common/store'

//����ȫ��uView
import uView from 'uview-ui';
Vue.use(uView);


// ����uView��С��������mixin��װ
let mpShare = require('@/uview-ui/libs/mixin/mpShare.js');
Vue.mixin(mpShare)

//����ȫ��colorUIͷ��
import cuCustom from '@/static/colorui/components/cu-custom.vue'
Vue.component('cu-custom', cuCustom)

import { apiFilesUrl } from '@/common/setting/constVarsHelper.js'


Vue.config.productionTip = false
Vue.prototype.$upload = Upload;
Vue.prototype.$common = Common;
Vue.prototype.$db = Db;
Vue.prototype.$config = Config;
Vue.prototype.$store = store;

Vue.prototype.$apiFilesUrl = apiFilesUrl;

App.mpType = 'app'

const app = new Vue({
    ...App
})

// http������
import httpInterceptor from '@/common/request/http.interceptor.js'
// ������Ҫд�������Ϊ�˵�Vue����������ɣ�����"app"����(Ҳ��ҳ���"this"ʵ��)
Vue.use(httpInterceptor, app)

// http�ӿ�API���й������벿��
import httpApi from '@/common/request/http.api.js'
Vue.use(httpApi, app)

app.$mount()
