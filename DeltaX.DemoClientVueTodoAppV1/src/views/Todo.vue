<template>
  <div class="container mx-auto py-3"> 
    <div v-show="loading" class="h-2 relative max-w rounded-full overflow-hidden progressbar indeterminate">
      <div class="w-full h-full bg-gray-200 absolute" />
      <div class="h-full bg-green-500 absolute progressbar" style="width:10%" role="progressbar" />
    </div> 
    <div v-if="!loading && ![200, 201, 0].includes(status.state)" class="text-white px-6 py-2 border-0 rounded relative mb-4 bg-red-400"> 
      <span class="inline-block align-middle mr-8">
        <b>{{status.state}}</b> {{status.message}}
      </span> 
    </div>

    <div class="new-todo-form flex column my-2">
      <input
        class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        type="text"
        v-model="newTodo"
        placeholder="Create new todo" 
        @keyup.enter="addTodo">
      <button 
        class="bg-white hover:bg-blue-100 text-gray-800 font-semibold ml-2 py-2 px-2 ripple border border-gray-400 rounded shadow" 
        @click="addTodo" > 
        <svg class="w-5 h-5 text-red fill-current" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="3" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
        </svg>
      </button> 
    </div>
    <div v-if="todos" > 
      <ul v-for="t in todos.items" :key="t.id"   >
        <TodoItem :todo="t" @update="onUpdate" @remove="onRemove" />
      </ul>
    </div>
  </div>
</template>

<script lang="ts">
import { ref, watchEffect } from 'vue'
import { todos, getTodos, status, updateTodo, createTodo, deleteTodo } from "@/composables/useTodoRepository";
import TodoItem from "@/components/TodoItem.vue"

export default {
  components:{
    TodoItem
  },

  setup () { 
    const loading = ref(false)
    const newTodo = ref("") 
   
    watchEffect(() => { 
      if(status.value.loading) loading.value = true;
      else if (loading.value) {
        setTimeout(()=> loading.value = status.value.loading, 200)
      }
    }) 

    const addTodo = async function () { 
      await createTodo({description: newTodo.value, completed: false})
      newTodo.value = ""; 
    }

    const onUpdate =  (id: number, description: string, completed: boolean)=> { 
      console.log("onCompleted", id, description, completed);
      const todo = todos.value?.items.find(t => t.id == id)
      if(todo)
      {
        updateTodo({id : id, description : description, completed : completed })
      }
    }
    
    const onRemove =  (id: number)=> { 
      console.log("onRemove", id);
      const todo = todos.value?.items.find(t => t.id == id)
      if(todo)
      {
        deleteTodo(id)
      }
    }

    return {
      loading,
      newTodo,
      addTodo,
      getTodos,
      todos,  
      status,  
      onUpdate,
      onRemove
    }
  }
}
</script>

<style scoped>
@keyframes progress-indeterminate {
  0% {
    width: 20%;
    left: -40%;
  }
  30% {
    width: 40%;
    left: -40%;
  }
  60% {
    width: 80%;
    left: -40%;
  }
  90% {
    left: 100%;
    width: 100%;
  }
  to {
    left: 100%;
    width: 0;
  }
}
.progressbar {
  transition: width 0.2s ease;
}
.indeterminate .progressbar {
  animation: progress-indeterminate 0.8s ease infinite;
}
</style>