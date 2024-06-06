<template>
    <div class="container mt-5 overflow-x-auto">
        <h1 class="ms-3">Superpowers</h1>
        <div class="d-inline-flex justify-content-center">
        <Superpower v-for="p in powers" :power="p" @activate="OnActivate(p.id, p)" @disactivate="OnDisactivate" class="mx-3"></Superpower>
    </div>
</div>
</template>

<script setup lang="ts">
import { ConnectSuperpower, GetAll } from '@/api/superpowers';
import type SuperpowerModel from '@/models/SuperpowerModel';
import { onMounted, ref } from 'vue'

import Superpower from '@/components/Superpower.vue';


const powers = ref([] as Array<SuperpowerModel>)

onMounted(async () => {
    const newPowers = await GetAll(); 
    powers.value.push(... newPowers.Get());
})

async function OnActivate(superpowerId: number, p: SuperpowerModel){
    let res = await ConnectSuperpower(superpowerId)
    if(res.success && res.Get())
        p.isActive = true;
}

function OnDisactivate(){
    
}
</script>