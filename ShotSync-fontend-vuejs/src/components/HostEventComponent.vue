<template lang="">
  <q-intersection class="host-intersection-item">
    <q-card flat bordered class="q-ma-sm cursor-pointer">
      <q-img src="https://cdn.quasar.dev/img/mountains.jpg" img-class="item-background-image">
        <div
          class="text-h6 absolute-top text-center bg-transparent text-uppercase text-bold text-italic"
        >
          {{ eventName }}
        </div>
        <div
          class="text-subtitle2 absolute-bottom text-center bg-transparent text-uppercase text-bold text-italic"
        >
          <div>{{ eventVenue }}</div>
          <div class="text-secondary">{{ eventLoc }}</div>
        </div>
        <div
          class="absolute-top-right text-subtitle2 text-italic text-bold bg-positive"
          style="border-radius: 0 0 0 10px"
          v-if="eventIsHappening"
        >
          Taking Place
        </div>
      </q-img>
      <q-card-actions>
        <div class="row" style="width: 100%">
          <div class="col-8">
            <q-btn flat round icon="event" />
            <q-btn flat> {{ eventTime }} </q-btn>
          </div>
          <div class="col-4 column text-right justify-center">
            <q-btn class="bg-secondary" unelevated rounded color="primary" v-if="!eventIsHappening">
              View Result
            </q-btn>
            <q-btn class="bg-secondary" unelevated rounded color="primary" v-if="eventIsHappening">
              View Event</q-btn
            >
          </div>
        </div>
      </q-card-actions>
    </q-card>
  </q-intersection>
</template>
<script setup lang="ts">
export interface EventComponentProps {
  eventName: string
  eventLoc: string
  eventTime: string
  eventVenue: string
  eventIsHappening: boolean
}

withDefaults(defineProps<EventComponentProps>(), {
  eventName: '',
  eventLoc: '',
  eventTime: '',
  eventVenue: '',
  eventIsHappening: false,
})
</script>
<style lang="sass" scoped>
.host-intersection-item
    height: 350px
    width: 23%
</style>

<style lang="sass">
.item-background-image
    filter: blur(1px) grayscale(90%) brightness(70%) contrast(120%)
</style>
