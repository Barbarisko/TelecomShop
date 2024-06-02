import './assets/main.scss'

import * as bootstrap from 'bootstrap'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate'

const app = createApp(App)

console.log(import.meta.env.VITE_TEST)

const pinia = createPinia();
pinia.use(piniaPluginPersistedstate)

app.use(pinia)
    .use(router)

app.mount('#app')
