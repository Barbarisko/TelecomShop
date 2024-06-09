<template>
    <div class="container mt-4">
        <Usage :metric="'SMS'" :limit="user?.smsLimit" :used="user?.smsBalance"/>
        <Usage :metric="'Internet'" :limit="user?.internetLimit" :used="user?.internetBalance"/>
        <Usage :metric="'Voice'" :limit="user?.voiceLimit" :used="user?.voiceBalance"/>

        <div class="row">
            <div class="col">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Your number: {{user?.phoneNumber}}</h5>
                    <p class="card-text">Cool number!</p>
                </div>
            </div>
        </div>
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <div class="d-flex justify-content-between">
                            <div>
                                <h5 class="card-title">{{ user?.balance }} uah</h5>
                                <p class="card-text">Pls pay us more</p>
                            </div>
                            
                            <a href="#" class="btn btn-primary" 
                                style="display: flex; align-items: center;justify-content: center;">
                                Top up the balance
                            </a>
                    </div>
                </div>
            </div>
        </div>
        
    </div>
    <div class="row mt-3">
        <div class="col-8">
            <h3>Your current plan:</h3>
            <Plan :plan="activePlan" :hardcoded-width="false"></Plan>
        </div>
        <div class="col">
            <h3>Your current superpowers:</h3>
            <div v-if="activepowers.length==0">
                <p>There are currently no superpowers activated. Would you like some?</p>
                </div>
            <RouterLink class="btn btn-primary" to="/superpowers" 
                            style="display: flex; align-items: center;justify-content: center;">
                            Select Superpowers
            </RouterLink>
            <div v-for="p in activepowers" class="container">
            <Superpower :power="p" :hardcoded-width="false" :show-buttons="false"
                class="my-2">
            </Superpower>
            </div>            
        </div>
    </div>
</div>
</template>

<script setup lang="ts">
import * as SP_API from '@/api/superpowers';
import * as PLAN_API from '@/api/plans';
import { GetStats } from '@/api/users';
import Plan from '@/components/Plan.vue';
import Superpower from '@/components/Superpower.vue';
import Usage from '@/components/Usage.vue';
import type PlanModel from '@/models/PlanModel';
import type SuperpowerModel from '@/models/SuperpowerModel';
import UserStats from '@/models/UserModel';
import { onMounted, ref } from 'vue';

const user = ref<UserStats | undefined>(undefined)
const activepowers = ref<Array<SuperpowerModel>>([])
const activePlan = ref<PlanModel | undefined>(undefined)

onMounted(async () => {
    user.value = (await GetStats()).Get(); 
    activepowers.value.push(... (await SP_API.GetCurrent()).Get());
    activePlan.value = (await PLAN_API.GetCurrent()).Get();
})
</script>