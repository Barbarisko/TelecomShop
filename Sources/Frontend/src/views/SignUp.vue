<template>
  <div>
    <h1>This is an signup page</h1>
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
                <label for="name">Name</label>
                <input v-model="name" type="text" class="form-control" id="name">
                
                <label for="surname">Surname</label>
                <input v-model="surname" type="text" class="form-control" id="surname">
              </div>
              <div class="form-group">
                <label for="exampleInputPassword1">Password</label>
                <input v-model="password" type="password" class="form-control" id="exampleInputPassword1">
              </div>
              <div class="form-group">
                <label for="exampleInputPassword2">Password</label>
                <input v-model="passwordRepeated" type="password" class="form-control" id="exampleInputPassword2">
              </div>
              <button type="submit" :disabled="!canSubmit" class="btn btn-primary">Submit</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">

import { computed, ref } from 'vue'
import { useLoginStore } from '@/stores/login'
import { useRouter } from 'vue-router';
import { SignUp } from '@/api/login'

const loginStore = useLoginStore()

const phoneNumber = ref("");
const password = ref("");
const name = ref("");
const surname = ref("");
const passwordRepeated = ref("");

const canSubmit = computed(() => {
  return phoneNumber.value != "" 
  && name.value != ""
  && surname.value != ""
  && password.value != ""
  && password.value == passwordRepeated.value;
})

const router = useRouter();

const handleSubmit = async () => {
  const res = await SignUp(phoneNumber.value, password.value, name.value, surname.value);
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
