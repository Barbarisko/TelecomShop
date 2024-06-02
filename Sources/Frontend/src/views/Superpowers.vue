<template>
    <div class="container mt-5 overflow-x-auto">
        <h1 class="ms-3">Superpowers</h1>
        <div class="d-inline-flex justify-content-center">
        <!-- <Plan v-for="p in plans" :plan="p" class="mx-3"></Plan> -->
    </div>
</div>
</template>

<script setup lang="ts">
import { GetAll } from '@/api/superpowers';
import type SuperpowerModel from '@/models/SuperpowerModel';
import { onMounted, ref } from 'vue'

import Plan from '@/components/Plan.vue'


const plans = ref([] as Array<SuperpowerModel>)

onMounted(async () => {
    const newPlans = await GetAll(); 
    plans.value.push(... newPlans.Get());
})
</script>