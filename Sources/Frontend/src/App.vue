<script setup lang="ts">
import { RouterView, useRouter } from 'vue-router'
import { useLoginStore } from '@/stores/login'
import ToastView from './views/ToastView.vue';


const loginStore = useLoginStore();
const router = useRouter();
function LogOut()
{
  loginStore.logOut();
  router.push("/login")
}
function LogIn()
{
  router.push("/login")
}
function SignUp()
{
  router.push("/signup")
}
</script>

<template>

  <header class="p-3 text-bg-dark">
    <div class="container">
      <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
        <RouterLink to="/" class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none">
          <svg class="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap"><use xlink:href="#bootstrap"></use></svg>
        </RouterLink>

        <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
          <li><RouterLink class="nav-link px-2 text-white" to="/home">Home</RouterLink></li>
          <li><RouterLink class="nav-link px-2 text-white" to="/plans">Plans</RouterLink></li>
          <li><RouterLink class="nav-link px-2 text-white" to="/superpowers">Superpowers</RouterLink></li>
          <li><RouterLink class="nav-link px-2 text-white" to="/about">About</RouterLink></li>
        </ul>

        <div class="text-end">
          <div v-if="loginStore.isLoggedIn">
            Hello {{ loginStore.name }} {{ loginStore.surname }}!
            <button type="button" @click="LogOut" class="ms-2 btn btn-outline-warning me-2">Logout</button>
          </div>
          <div v-else>
            <button type="button" @click="LogIn" class="btn btn-outline-light me-2">Login</button>
            <button type="button" @click="SignUp" class="btn btn-warning">Sign-up</button>
          </div>
        </div>
      </div>
    </div>
  </header>

  <RouterView />
  <ToastView />
</template>