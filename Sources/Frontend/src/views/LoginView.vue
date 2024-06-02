<template>
  <div>
    <h1>This is an login page</h1>
  </div>
  <div class="container">
    <div class="row">
      <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-4 offset-lg-4">
        <div class="card">
          <div class="card-body">
            <form @submit.prevent="handleSubmit">
              <div class="form-group">
                <label for="phoneNumber">Phone number</label>
                <input v-model="phoneNumber" type="text" class="form-control" id="phoneNumber">
              </div>
              <div class="form-group">
                <label for="exampleInputPassword1">Password</label>
                <input v-model="password" type="password" class="form-control" id="exampleInputPassword1">
              </div>
              <button type="submit" class="btn btn-primary">Submit</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref } from 'vue'
import { useLoginStore } from '@/stores/login'
import { useRouter } from 'vue-router';
import { LogIn } from '@/api/login'

const loginStore = useLoginStore()

const phoneNumber = ref("");
const password = ref("");

const router = useRouter();

const handleSubmit = async () => {
  const res = await LogIn(phoneNumber.value, password.value);
  if (!res.success) {
    return;
  }
  if (!loginStore.logIn(res.Get().token)) {
    return;
  }

  router.push('/home');
} 
</script>

<style></style>
