<template lang="">
  <q-intersection class="intersection-item">
    <q-card flat bordered class="q-ma-sm cursor-pointer">
      <q-img src="https://cdn.quasar.dev/img/mountains.jpg" img-class="item-background-image">
        <div
          class="text-h6 absolute-top text-center bg-transparent text-uppercase text-bold text-italic"
        >
          {{ tournamentName }}
        </div>
        <div
          class="text-subtitle2 absolute-bottom text-center bg-transparent text-uppercase text-bold text-italic"
        >
          <div>{{ tournamentVenue }}</div>
          <div class="text-secondary">{{ tournamentLoc }}</div>
        </div>
        <div
          class="absolute-top-right text-subtitle2 text-italic text-bold bg-positive"
          style="border-radius: 0 0 0 10px"
          v-if="tournamentIsHappening"
        >
          Taking Place
        </div>
      </q-img>
      <q-card-actions>
        <div class="row" style="width: 100%">
          <div class="col-8">
            <q-btn flat round icon="event" />
            <q-btn flat> {{ tournamentTime }} </q-btn>
          </div>
          <div class="col-4 column text-right justify-center">
            <q-btn
              class="bg-secondary"
              unelevated
              rounded
              color="primary"
              v-if="!tournamentIsHappening"
            >
              View Result
            </q-btn>
            <q-btn
              class="bg-secondary"
              unelevated
              rounded
              color="primary"
              v-if="tournamentIsHappening"
            >
              View Tournament</q-btn
            >
          </div>
        </div>
      </q-card-actions>
    </q-card>
  </q-intersection>
</template>
<script setup lang="ts">
export interface TournamentComponentProps {
  tournamentName: string
  tournamentLoc: string
  tournamentTime: string
  tournamentVenue: string
  tournamentIsHappening: boolean
}

withDefaults(defineProps<TournamentComponentProps>(), {
  tournamentName: '',
  tournamentLoc: '',
  tournamentTime: '',
  tournamentVenue: '',
  tournamentIsHappening: false,
})
</script>
<style lang="sass" scoped>
.intersection-item
    height: 400px
    width: 500px
</style>
