import { createApp, h } from 'vue'
import { DefineComponent } from 'vue'
import { createInertiaApp } from '@inertiajs/vue3'
import Layout from './common/Layout.vue'
import './assets/css/app.css'
import './assets/css/buttons.scss'
import './assets/css/form.scss'
import './assets/css/reset.scss'

createInertiaApp({
  id: 'app',
  resolve: (name) => {
    const pages = import.meta.glob('./Pages/**/*.vue', { eager: true })
    let page = pages[`./Pages/${name}.vue`] as DefineComponent
    page.default.layout = page.default.layout || Layout
    return page
  },
  setup({ el, App, props, plugin }) {
    createApp({ render: () => h(App, props) })
      .use(plugin)
      .mount(el)
  },
})
