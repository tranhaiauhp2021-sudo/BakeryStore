document.addEventListener('DOMContentLoaded', function(){
 var menu=document.querySelector('.mobile-menu-toggle'),nav=document.querySelector('#mainNav');
 if(menu && nav)menu.addEventListener('click',function(){var open=nav.classList.toggle('open');menu.setAttribute('aria-expanded',String(open));menu.querySelector('i').className=open?'fas fa-times':'fas fa-bars';});
 document.querySelectorAll('.qty').forEach(function(box){var el=box.querySelector('input'),down=box.querySelector('.qty-minus'),up=box.querySelector('.qty-plus');if(!el)return;
  if(down)down.addEventListener('click',function(){el.value=Math.max(1,parseInt(el.value||'1',10)-1);});if(up)up.addEventListener('click',function(){el.value=Math.min(99,parseInt(el.value||'1',10)+1);});
 });
});
