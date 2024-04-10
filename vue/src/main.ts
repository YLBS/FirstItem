import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'

import axios from 'axios'


import mitss from 'mitt'
const Mit=mitss();


import * as Elicons from "@element-plus/icons-vue";
const app=createApp(App);
app.config.globalProperties.$Bus = Mit

declare module 'vue' {
  export interface ComponentCustomProperties {
    $Bus: typeof Mit
  }
}
app.use(router);
app.use(ElementPlus);
app.provide('$axios',axios)
for (const [key, component] of Object.entries(Elicons)) {
    app.component(key, component)
  }
app.mount('#app')
