<template>
  <q-layout class="row justify-center">
    <div class="col"></div>
    <div class="col-10 q-py-md column">
      <q-spinner v-if="loading" color="primary" />
      <div
        class="event-infor-component w-100 bg-no-2 q-my-lg row"
        style="height: auto; border-radius: 20px; width: 100%"
      >
        <div class="col column justify-center items-center">
          <div
            class="row bg-primary text-white text-h6 justify-center"
            style="height: 40px; width: 30%; border-radius: 0 0 10px 10px"
          >
            EVENT INFOR
          </div>
          <div
            class="row text-h3 text-weight-medium text-italic text-uppercase text-primary q-py-lg q-my-lg"
          >
            {{ event.name }}
          </div>
          <div class="row text-white justify-around" style="width: 100%">
            <div class="column items-center justify-center q-ma-lg">
              <span class="row items-center text-h6"
                ><q-icon name="calendar_today" class="q-mr-sm" /> Date</span
              >
              <span class="text-h6 text-primary">{{ dateFormater(event.date) }}</span>
            </div>
            <div class="column items-center justify-center">
              <span class="row items-center text-h6"
                ><q-icon name="location_on" class="q-mr-sm" />Venue</span
              >
              <span class="text-h6 text-primary">{{ event.venue }}</span>
            </div>
            <div class="column items-center justify-center">
              <span class="row items-center text-h6"
                ><q-icon name="public" class="q-mr-sm" />Location</span
              >
              <span class="text-h6 text-primary">{{ event.location }}</span>
            </div>
          </div>
        </div>
      </div>
      <q-page-container class="bg-no-4" style="border-radius: 20px">
        <q-tabs v-model="selectedTab" inline-label class="text-primary custom-tabs">
          <q-tab
            v-for="(tab, index) in tabs"
            :key="index"
            :name="tab.name"
            :label="tab.label"
            class="custom-tab-label"
            @click="eventTabsHandle(tab.tabDes)"
          />
        </q-tabs>
        <div class="q-px-md">
          <router-view />
        </div>
      </q-page-container>
    </div>
    <div class="col"></div>
  </q-layout>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { ref, onMounted, watch } from 'vue'
import type { Event } from '../components/models'
import { dateFormater } from '../helper/DateFormater'


import { pathSegmentation } from '../helper/PathSegmentation'

const route = useRoute()
const router = useRouter()

const eventName = route.params.eventName
const eventNameStr = (Array.isArray(eventName) ? eventName[0] : eventName || 'error') as string

const loading = ref(true)

const tabs = ref([
  { name: 'player-list', label: 'Players list', tabDes: '' },
  { name: 'event-live-score', label: 'Live scores', tabDes: 'live-score' },
  { name: 'event-branches', label: 'Branches', tabDes: 'brackets' },
  { name: 'event-rankings', label: 'Rankings', tabDes: 'rankings' },
])
const selectedTab =
  pathSegmentation(route.path) == '' ? ref('player-list') : ref(pathSegmentation(route.path))

const event = ref<Event>({
  id: 0,
  name: '',
  venue: '',
  location: '',
  date: '',
  isHappen: false,
})

import EventService from 'src/services/event.service'

const fetchEvents = async () => {
  try {
    const events = await EventService.getEventsByName(eventNameStr)
    if (events && events.length > 0) {
      const e = events[0]!
      event.value = {
        id: e.id,
        name: e.name || '',
        venue: e.venue || '',
        location: e.location || '',
        date: e.date,
        isHappen: e.isHappen
      }
    }
  } catch (error) {
    console.error('Error fetching data:', error)
  } finally {
    loading.value = false
  }
}

const eventTabsHandle = async (tabDes: string) => {
  // Determine base path dynamically based on current route
  const currentPath = route.path;
  const isHost = currentPath.startsWith('/host');
  const basePath = isHost ? '/host/events' : '/player/events';

  // Construct new route
  // If tabDes is empty, it's the root (EventDetail)
  const separator = tabDes ? '/' : '';
  const newRoute = `${basePath}/${eventNameStr}${separator}${tabDes}`
  await router.push(newRoute)
}

onMounted(async () => {
  await fetchEvents()
})

watch(route, (newRoute) => {
  const currPath = pathSegmentation(newRoute.path)
  selectedTab.value = currPath == '' || currPath == undefined ? 'player-list' : currPath
})
</script>

<style lang="scss">
.custom-tabs .q-tab.q-tab--active {
  background-color: #eae9ea;
}
.custom-tab-label .q-tab__label {
  font-size: 18px;
}
</style>
