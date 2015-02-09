var app_names = ['<img class="store_icon" src="img/icon/android.png"/>손안에서 둘러보는 <br/> 우리나라 특산물<br/>초등 사회(3~6)', '<img class="store_icon" src="img/icon/android.png"/>스마트 사이언스<br/>초등 과학(3)', '<img class="store_icon" src="img/icon/android.png"/>전래동화와 함께<br/>떠나는 수학나라<br/>초등 수학(4)', '<img class="store_icon" src="img/icon/android.png"/>가야의 땅 경남<br/>초등 사회(4)', '<img class="store_icon" src="img/icon/android.png"/>지구와 달<br/>초등 과학(5)', '<img class="store_icon" src="img/icon/android.png"/>위인이 들려주는<br/> 역사 이야기<br/>초등 사회(5)', '<img class="store_icon" src="img/icon/android.png"/>역사 속 인물탐구<br/>초등 사회(5)', '<img class="store_icon" src="img/icon/android.png"/>스마트 실험실<br/>초등 과학(6)', '<img class="store_icon" src="img/icon/android.png"/>한국사 타임머신<br/>초,중,고(공통)', '<img class="store_icon" src="img/icon/android.png"/>사회과부도<br/>중(1~2)', '<img class="store_icon" src="img/icon/android.png"/>학교생활<br/>퀵 가이드<br/>초등', '<img class="store_icon" src="img/icon/ios.png"/>무지개 학급경영<br/>초등(1~6)', '<img class="store_icon" src="img/icon/pdf.png"/>모듬학습<br/>도구 Top 21<br/>초,중,고(공통)', '<img class="store_icon" src="img/icon/pdf.png"/>내꺼야?<br/>네꺼야?<br/>초등','<img class="store_icon" src="img/icon/pdf.png"/>내 손안의<br/>스마트 2.0<br/>초,중,고','<img class="store_icon" src="img/icon/android.png"/>좋은책<br/>초,중,고'];
var app_ids = ['pe.giulim.specialities1', 'com.moon.recycler', 'com.g11tg.Company.ProductName', 'zz_gimhae.exitschool.kr', 'com.moon.earth.moon', 'com.knowledgeware.modelexecutor.greatman_1410220155', 'com.distoma.personedu', 'air.SmartExperiment', 'kr.go.gne.time.machine.korean.history', 'appinventor.ai_jhujubi.k', 'com.knowledgeware.modelexecutor.teach_helper_1410310126','kr.go.gne.goodbooks'];
var appId;

function init() {

	if (location.search.length > 1) {
		appId = location.search.split('id=')[1]
	}

	$(".icon2").attr('src', 'img/icon/' + appId + '.png');
	$("#app_content").attr('src', 'img/content/' + appId + '.jpg');
	$("#app_name").html(app_names[appId - 1]);

	if (appId > 12)
		$("#app_down").attr('src', 'img/down_pdf.png');
	if (appId == 16)
		$("#app_down").attr('src', 'img/down_app.png');
}


$(".btn_back").click(function(event) {
	window.location.href = "index.html";
});
$("#app_down").click(function(event) {
	var down_link;
	if (appId < 12) {
		down_link = "https://play.google.com/store/apps/details?id=" + app_ids[appId - 1];
	} else if (appId == 12) {
		down_link = "https://itunes.apple.com/kr/app/leinbou-haggeubgyeong-yeong/id933807155?mt=8";
	} else if (appId > 12 && appId <16) {
		down_link = "pdf/" + appId+".pdf";
	} else if (appId == 16) {
		down_link = "https://play.google.com/store/apps/details?id=" + app_ids[11];
	}
	window.open(down_link);
	// window.location.href = down_link;
});

$("#app_ppt").click(function(event) {
	var down_ppt_link;
	if(appId<15)
		down_ppt_link = 'ppt/' + appId + '.pptx';
	else
		down_ppt_link = 'img/content/' + appId + '.jpg';
	window.open(down_ppt_link);
});

init();

