import Vue from 'vue';
import App from './App.vue';
import store from "./store/store";
import BootstrapVue from 'bootstrap-vue';
import router from './router/index.js';
import requestInterceptor from './Interceptors/RequestInterceptor';
import responseInterceptor from './Interceptors/ExceptionResponseInterceptor';

Vue.use(BootstrapVue)
Vue.use(router)

requestInterceptor();
responseInterceptor();

new Vue({
  store,
  router,
  BootstrapVue,
  render: h => h(App)
}).$mount('#app');