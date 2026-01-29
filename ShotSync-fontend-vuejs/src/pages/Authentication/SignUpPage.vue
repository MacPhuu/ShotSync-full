<template>
  <q-layout class="bg-no-1 window-height window-width row justify-center items-center">
    <div class="column">
      <div class="row items-center justify-center">
        <div class="text-h3 text-secondary text-bold">ShotSync Ranking.</div>
      </div>
      <div class="row items-center justify-center">
        <div class="text-h6 text-white q-mb-lg">
          Welcome to the <span class="text-secondary">ShotSync Ranking.</span> live scoring.
        </div>
      </div>
      <div class="row">
        <q-card square bordered class="q-pa-md shadow-1">
          <q-card-section>
            <div class="text-h5 text-primary text-bold q-pb-md">Sign Up</div>
            <q-form class="q-gutter-md">
              <q-input square filled clearable v-model="email" type="email" label="email" />
              <q-input square filled clearable v-model="username" type="text" label="username" />
              <q-input
                square
                filled
                clearable
                v-model="password"
                type="password"
                label="password"
              />
            </q-form>
          </q-card-section>
          <q-card-actions class="q-px-md">
            <q-btn unelevated size="lg" class="full-width bg-primary text-white" label="Sign Up" />
          </q-card-actions>
          <q-card-section class="text-center q-pa-none">
            <p class="text-grey-6">Already reigistered? Login</p>
          </q-card-section>
        </q-card>
      </div>
    </div>
  </q-layout>
</template>

<script lang="ts" setup>
import { ref } from 'vue'

const email = ref('')
const password = ref('')
const username = ref('')
</script>

<style>
.q-card {
  width: 450px;
}
</style>
