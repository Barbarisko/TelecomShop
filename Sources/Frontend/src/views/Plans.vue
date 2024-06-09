<template>
    
    <div class="container mt-5 overflow-x-auto">
        <h1 class="ms-3">Plans</h1>
        <div class="d-inline-flex justify-content-center">
            <div v-for="p in plans" class="d-flex flex-column align-items-center">
                <Plan :plan="p" class=" mx-3"></Plan>
                <button @click="openSelectedPlan(p.id)" class="btn btn-primary w-75 mt-2">Select</button>
            </div>
        </div>
    </div>

    <div v-if="selectedPlan != undefined" class="container mt-5">
        <PlanConfiguration :hardcoded-width="false" :plan="selectedPlan" @submit="onUpdatePlan"></PlanConfiguration>
    </div>
</template>

<script setup lang="ts">
import { GetAll, SelectNewPlan, ChangePlan } from '@/api/plans';
import type PlanModel from '@/models/PlanModel';
import { onMounted, ref } from 'vue'

import Plan from '@/components/Plan.vue'
import SelectedPlanModel from '@/models/SelectedPlanModel';
import PlanConfiguration from '@/components/PlanConfiguration.vue';
import { useRouter } from 'vue-router';
import { ToastLevel, useToastStore } from '@/stores/toast';
import { error } from 'console';

const toastStore = useToastStore();
const router = useRouter();


const plans = ref([] as Array<PlanModel>);

// let testMap: Map<string, string> = new Map<string, string>();
// // Step 2: Fill the Map with test data
// testMap.set('key1', 'value1');
// testMap.set('key2', 'value2');
// testMap.set('key3', 'value3');
// testMap.set('key4', 'value4');
// testMap.set('key5', 'value5');

// let testMapListValues: Map<string, string> = new Map<string, string>();
// // Step 2: Fill the Map with test data
// testMapListValues.set('key1', 'list1, list2, list3');
// testMapListValues.set('key2', 'list1, list3');

onMounted(async () => {
    const newPlans = await GetAll(); 
    plans.value.push(... newPlans.Get());
})

const selectedPlan = ref<SelectedPlanModel | undefined>(undefined)

async function openSelectedPlan(id: number){
    selectedPlan.value = (await SelectNewPlan(id)).Get();
}

async function onUpdatePlan(payload: { id: number, chars: Map<string, string>}) {
    let res = await ChangePlan(payload.id, payload.chars);
    if(res.success && res.Get())
    {
        toastStore.showToast({level:ToastLevel.Info, title:"Success", message:"Plan changed successfully"})
        router.push('/home')
    }
    else {
        toastStore.showToast({level:ToastLevel.Error, title:"Error", message:res.error})
    }

}
</script>