<template>
  <q-page class="row justify-center q-pa-lg">
    <div
      class="col-5 text-center bg-no-4 q-ma-sm q-pa-xl"
      style="max-height: 500px; max-width: 500px"
    >
      <q-img
        :src="url"
        spinner-color="white"
        :ratio="1"
        style="max-height: 300px; max-width: 300px; border-radius: 50%"
      />
      <div class="q-mt-lg text-h4 text-primary">Host Name</div>
    </div>
    <div class="col text-center bg-no-4 q-ma-sm" style="max-height: 500px; max-width: 700px">
      <div class="text-h5 text-bold q-pa-lg">INFORMATION</div>
      <div class="row q-pa-lg">
        <div class="col text-left text-italic" style="padding-left: 20px">
          <div class="text-h7 text-gray">Address:</div>
          <div class="text-h6 q-mb-md">Ohio, America</div>
          <div class="text-h7 text-gray">Phone Number:</div>
          <div class="text-h6 q-mb-md">0995995995</div>
          <div class="text-h7 text-gray">Email:</div>
          <div class="text-h6 q-mb-md">ohiobilliard@gmail.com</div>
          <div class="text-h7 text-gray">Owner:</div>
          <div class="text-h6 q-mb-md">John Doe</div>
          <div class="text-h7 text-gray">Owner Contact(Email/Phone):</div>
          <div class="text-h6 q-mb-md">086868686</div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue'
const url = ref('https://picsum.photos/500/300')
</script>
