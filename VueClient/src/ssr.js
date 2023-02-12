import { createInertiaApp } from '@inertiajs/vue3'
import createServer from '@inertiajs/vue3/server'

import { renderToString } from '@vue/server-renderer'
import { createSSRApp, DefineComponent, h } from 'vue'
import Layout from './common/Layout.vue'

createServer((page) =>
  createInertiaApp({
    page,
    render: renderToString,
    resolve: (name) => {
      const pages = import.meta.glob('./Pages/**/*.vue', { eager: true })
      const page = pages[`./Pages/${name}.vue`]
      page.default.layout = page.default.layout || Layout
      return page
    },
    setup({ App, props, plugin }) {
      return createSSRApp({
        render: () => h(App, props),
      }).use(plugin)
    },
  })
)
