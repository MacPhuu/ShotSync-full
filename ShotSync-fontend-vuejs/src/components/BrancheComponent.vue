<template>
  <div class="player-component bg-no-2 q-my-sm row" style="height: 100px; border-radius: 20px">
    <div class="col-5 text-right self-center q-pl-md text-h4 text-bold">{{ firstPlayerName }}</div>
    <div class="col text-center self-center q-pl-md text-h4 text-bold">
      <div style="position: relative" v-if="!isFinish && isHost">
        <q-btn
          style="position: absolute; top: -25px; left: 35px"
          round
          size="8px"
          color="positive"
          icon="add"
          @click="plusFirstPlayerPoint"
        />
        <q-btn
          style="position: absolute; top: 40px; left: 35px"
          size="8px"
          round
          color="negative"
          icon="remove"
          @click="minusFirstPlayerPoint"
        />
      </div>
      <span :class="{ 'text-red': isFinish && firstPlayerPoint > secondPlayerPoint }">{{
        firstPlayerPoint
      }}</span>
      -

      <span :class="{ 'text-red': isFinish && firstPlayerPoint < secondPlayerPoint }">{{
        secondPlayerPoint
      }}</span>
      <div style="position: relative" v-if="!isFinish && isHost">
        <q-btn
          style="position: absolute; top: -65px; right: 35px"
          round
          size="8px"
          color="positive"
          icon="add"
          @click="plusSecondPlayerPoint"
        />
        <q-btn
          style="position: absolute; top: 0px; right: 35px"
          size="8px"
          round
          color="negative"
          icon="remove"
          @click="minusSecondPlayerPoint"
        />
      </div>
    </div>
    <div
      class="col-5 text-left self-center q-pr-md text-h4 text-bold"
      :class="{ 'text-red': secondPlayerName === 'Unknown' }"
    >
      {{ secondPlayerName }}
    </div>
    <div style="position: relative" v-if="!isFinish && isHost">
      <q-btn
        style="position: absolute; top: 12px; left: -130px; width: 120px"
        rounded
        color="info"
        @click="updateMatchPoint"
        label="Update Point"
      />
      <q-btn
        style="position: absolute; top: 53px; left: -130px; width: 120px"
        rounded
        color="primary"
        @click="finishMatch"
        label="Finish Match"
      />
    </div>
  </div>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import api from 'src/services/api'
import { Notify } from 'quasar'

const role = ref<number>(parseInt(localStorage.getItem('role') || sessionStorage.getItem('role') || '0'))
const isHost = role.value === 2

export interface BrancheComponentProps {
  id: number
  firstPlayerName: string
  firstPlayerPoint: number
  secondPlayerName: string
  secondPlayerPoint: number
  isFinish: boolean
}

const props = withDefaults(defineProps<BrancheComponentProps>(), {
  id: 0,
  firstPlayerName: '',
  firstPlayerPoint: 0,
  secondPlayerName: '',
  secondPlayerPoint: 0,
  isFinish: false,
})

const firstPlayerPoint = ref(props.firstPlayerPoint)
const secondPlayerPoint = ref(props.secondPlayerPoint)
const id = ref(props.id)

const plusFirstPlayerPoint = () => {
  firstPlayerPoint.value += 1
}

const minusFirstPlayerPoint = () => {
  if (firstPlayerPoint.value == 0) return
  firstPlayerPoint.value -= 1
}

const plusSecondPlayerPoint = () => {
  secondPlayerPoint.value += 1
}

const minusSecondPlayerPoint = () => {
  if (secondPlayerPoint.value == 0) return
  secondPlayerPoint.value -= 1
}

const updateMatchPoint = async () => {
  const updateInfo = {
    id: id.value,
    firstPlayerPoint: firstPlayerPoint.value,
    secondPlayerPoint: secondPlayerPoint.value,
  }
  const updateInfoJson = JSON.stringify(updateInfo)
  try {
    await api.put('matches/update-match-point', updateInfoJson)
    Notify.create({
      type: 'positive',
      position: 'top-right',
      message: 'Update successed!',
    })
  } catch (error) {
    console.error('Error fetching players:', error)
  }
}

const finishMatch = async () => {
  try {
    await api.put(`matches/match-finish/${id.value}`)
    Notify.create({
      type: 'positive',
      position: 'top-right',
      message: 'Update successed!',
    })
  } catch (error) {
    console.error('Error:', error)
  }
}
</script>
<style lang="scss">
.text-red {
  color: red;
}
</style>
