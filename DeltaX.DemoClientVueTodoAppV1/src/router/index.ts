import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import Home from '../views/Home.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/todo',
    name: 'Todo',
    component: () => import(/* webpackChunkName: "todo" */ '../views/Todo.vue')
  }
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router
