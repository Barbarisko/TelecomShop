<template>
    <div class="card" :style="hardcodedWidth ? 'width: 18rem;': ''">
        <img src="..." class="card-img-top" alt="...">
        <div class="card-body">
        <h5 class="card-title">{{ power?.name }}</h5>
        <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
        <a href="#" class="btn btn-primary">Go somewhere</a>
        </div>
    </div>
</template>

<script setup lang="ts">
import SuperpowerModel from '@/models/SuperpowerModel';

const props = defineProps({
    power: SuperpowerModel,
    hardcodedWidth: 
    {
        default: true,
        type: Boolean
    }
});

</script>
