<template>
    <div :style="hardcodedWidth ? 'width: 18rem;': ''">
        <div class="container px-5">
            <h5 class="card-title">{{ plan?.name }}</h5>
            <p class="card-text">{{plan?.description}}</p>
            <p class="fw-semibold">One-Time Price: {{plan?.priceOneTimeTotal}} UAH</p>
            <p class="fw-semibold">Recurrent Price: {{plan?.priceRecurrentTotal}} UAH</p>
            <p v-if="plan?.etf!=undefined " class="fw-semibold">Early Termination Fee: {{plan?.etf}} UAH</p>
            
            <div class="container">
                <ul class="list-group list-group-horizontal" v-for="char in props.plan?.characteristics">
                    <li class="list-group-item w-50">{{ char[0] }}</li>
                    
                    <li class="list-group-item w-50">
                        <div v-if="isOptionList(char[0])" class="dropdown">
                            <button class="btn btn-secondary dropdown-toggle w-100" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                {{ selectedOptions.get(char[0])?? "Please select value" }}
                            </button>
                            <ul class="dropdown-menu">
                                <li v-for="option in parsedOptionsMap.get(char[0])"><button @click="selectOption(char[0], option)" class="dropdown-item" href="#">{{option}}</button></li>
                            </ul>
                        </div>
                        <p v-else>
                            {{ (char[1]) }}
                        </p>
                        
                    </li>
                </ul>
            </div>

            <div class="container">
                <button @click="UpdatePlan()" class="btn btn-primary w-75 mt-2">Update</button>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import SelectedPlanModel from '@/models/SelectedPlanModel';
import { computed, ref } from 'vue';

const props = defineProps({
    plan: SelectedPlanModel,
    hardcodedWidth: 
    {
        default: true,
        type: Boolean
    }
});

const emit = defineEmits<{
  (e: 'submit', payload: { id: number, chars: Map<string, string> }): void
}>()


function isOptionList(key: string)
{
    if(props.plan === undefined){
        return false;
    } 
    return props.plan.characteristicListValues.has(key);
}
const emptyOptionsMap = new Map<string, Array<string>>();
const parsedOptionsMap = computed(() =>
{
    if(props.plan === undefined){
        return emptyOptionsMap;
    } 
    const map = new Map<string, Array<string>>();
    props.plan.characteristicListValues.forEach((value, key) => {
        map.set(key, value.split(", "));
    })
    return map;
})

const selectedOptions = ref(new Map<string, string>())
function selectOption(key: string, value: string)
{
    selectedOptions.value.set(key, value);
}

function UpdatePlan()
{
    if(props.plan == undefined)
        return;
    let newMap = new Map<string, string>();
    for(let [key, defaultVal] of props.plan.characteristics.entries())
    {
        if(props.plan?.characteristicListValues.has(key))
        {
            let value = selectedOptions.value.get(key);
            if(value == undefined) return;
            newMap.set(key, value);
        }
        else{

            newMap.set(key, defaultVal);
        }
    }    

    emit("submit", {id: props.plan.id, chars: newMap });
}

</script>
