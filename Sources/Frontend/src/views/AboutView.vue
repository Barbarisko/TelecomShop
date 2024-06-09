<template>
  <div class="about">
    <h1>Usage Statistics</h1>



    <div class="col">
      <div class="row">
        <datepicker
          v-model="selectedLowerDate"
          :upperLimit="selectedUpperDate"
          :clearable="true"
        />
        <datepicker
          v-model="selectedUpperDate"
          :lowerLimit="selectedLowerDate"
          :clearable="true"
        />
      </div>

        <div class="row">
          <button @click="generateStatsFile()" class="btn btn-success">
            Generate file
        </button>
        </div>

    </div>
  </div>
</template>

<script setup lang="ts">
import { GenerateFileStatistics } from "@/api/users";
import { ref } from "vue";
import datepicker from "vue3-datepicker"
import { ToastLevel, useToastStore } from '@/stores/toast';

const toastStore = useToastStore();

const selectedLowerDate = ref<Date | undefined>();
const selectedUpperDate = ref<Date | undefined>();

async function generateStatsFile() {
  if(selectedLowerDate.value == undefined || selectedUpperDate.value==undefined)
  {
    return;
  }

  let res = await GenerateFileStatistics(selectedLowerDate.value, selectedUpperDate.value);
    if(res.success)
    {
        toastStore.showToast({level:ToastLevel.Info, title:"Success", message:"File generated successfully! Please check your Downloads folder"})
    }
    else {
        toastStore.showToast({level:ToastLevel.Error, title:"Error", message:res.error})
    }}
</script>