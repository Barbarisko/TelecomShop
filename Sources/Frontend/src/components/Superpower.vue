<template>
    <div class="card" :style="hardcodedWidth ? 'width: 18rem;': ''">
        <!-- <img src="..." class="card-img-top" alt="..."> -->
        <div class="card-body">
        <h5 class="card-title">{{ power?.name }}</h5>
        <p class="card-text">{{power?.description}}</p>
        <p class="card-text">{{power?.priceOneTimeTotal}}</p>
        <div v-if="showButtons">
            <button v-if="power == undefined ? true : !power.isActive" href="#" @click="emit('activate')" class="btn btn-primary">Select</button>
            <button v-else href="#" @click="emit('disactivate')" class="btn btn-danger">Unselect</button>
        </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import SuperpowerModel from '@/models/SuperpowerModel';

const emit = defineEmits<{
  (e: 'activate'): void
  (e: 'disactivate'): void
}>()


const props = defineProps({
    power: SuperpowerModel,
    hardcodedWidth: 
    {
        default: true,
        type: Boolean
    },
    showButtons: 
    {
        default: true,
        type: Boolean
    }
});

</script>
