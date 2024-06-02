import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AboutView from '../views/AboutView.vue'
import LoginView from '../views/LoginView.vue'
import SignUp from '@/views/SignUp.vue'
import Plans from '@/views/Plans.vue'
import Superpowers from '@/views/Superpowers.vue'
import { useLoginStore } from '@/stores/login'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/home',
      name: 'home',
      component: HomeView
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: AboutView,
      //beforeEnter: authGuard,
    },
    {
      path: "/login",
      name: "login",
      component: LoginView,
    },
    {
      path: "/signup",
      name: "signup",
      component: SignUp,
    },
    {
      path: "/plans",
      name: "Plans",
      component: Plans,
    },
    {
      path: "/superpowers",
      name: "Superpowers",
      component: Superpowers,
    }
  ]
})

router.beforeEach(async (to, from) => {
  const loginStore = useLoginStore()

  if (
    // make sure the user is authenticated
    !loginStore.isLoggedIn
  ) {
    if(to.name !== 'signup' && to.name !== 'login' && to.name !== "about")
      // redirect the user to the login page
      return { name: 'login' }
  }
})

export default router
