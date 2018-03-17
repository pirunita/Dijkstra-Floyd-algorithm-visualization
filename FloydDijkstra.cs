using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
						//    경희대학교 정보전자신소재공학과 고성필
public class FloydDijkstra : MonoBehaviour {	//  - Unity를 이용한 Floyd & Dijkstra Algorithm 구현 -
    public Text startPointText;                         // 출발점을 표시하는 Text
    public Text endPointText;	                    // 최종 도착점을 표시하는 Text

    public Text selectNode1Text;                        // 가중치를 정하고싶은 edge의 출발 node
    public Text selectNode2Text;                        // 가중치를 정하고싶은 edge의 도착 node
    public Text weightText;                             // 부여하고싶은 가중치를 표시하는 Text
    Text edgeWeightText;                                // 경로위에 표시하는 가중치 Text

    public bool floydWork = false;                      // Floyd button switch
    public bool dijkstraWork = false;                   // Dijkstra button switch

    int startPoint = 0;                                 // 2개의 배열을 이용한 행렬을 만들 때 배열은 0부터 시작하므로 node v1을 0으로 잡는다
    int endPoint = 0;
    int selectNode1 = 0;                                // node간 edge에 weight를 부여할 때 출발하는 node를 표시하기 위한 값
    int selectNode2 = 0 ;                               // node간 edge에 weight를 부여할 때 도착하는 node를 표시하기 위한 값

    //public GameObject[] nodeImage = new GameObject[5];

    public GameObject[] edgeImage0 = new GameObject[5]; // 설정한 경로를 표시하기 위해 GameObject 배열 생성합니다.
    public GameObject[] edgeImage1 = new GameObject[5]; // 애니메이션이나 다른 방법으로 구현하려 했지만 실패하여
    public GameObject[] edgeImage2 = new GameObject[5]; // 일일이 경로를 미리 모두 프로그램에 넣어두어 사용자가 설정하면 해당 경로가 켜지도록 했습니다.
    public GameObject[] edgeImage3 = new GameObject[5]; // Node를 증가시킬 시 다시 추가해야되는 비효율적인 방법이므로 이를 개선해야 한다고 생각합니다.
    public GameObject[] edgeImage4 = new GameObject[5];


    int weight = 1;                                     // edge에 부여되는 weight 값을 1로 잡는다.
    int node = 4;                                       // 프로그램에 설정된 node의 수는 5로 고정되어 있지만 배열의 용이성을 위해 4로 잡는다.
  
    int[,]weightMatrix;                                 // Floyd Algorithm에 사용 될 edge의 가중치 행렬을 선언한다.
    int[,]D;                                            // Floyd Algorithm에 사용되는 최단경로의 길이가 포함된 행렬을 선언한다.
    int[,]Path;                                         // Floyd Algorithm에 사용되는 한 node에서 다른 node로 갈 때 최단비용으로 가기 위해 거쳐가는 node들의 행렬을 선언한다.
	
	                                                    /*---------------------------------------------------------------------------------------------------------------------*/
    int[]touch;                                         // Dijkstra Algorithm에 사용되는 한 node에서 다른 node로 갈 때 최단비용으로 가기 위해 거쳐가는 node들의 배열을 선언한다. 
    int[]length;                                        // Dijkstra Algorithm에 사용되는 한 node에서 다른 node로 갈 때 최단비용으로 갈 때의 weight를 저장하는 배열을 선언한다.
  
	// Use this for initialization
    void Start () {
        weightMatrix = new int[node + 1, node + 1]; // node의 수 크기에 해당하는 가중치 행렬 생성한다. 배열의 용이성을 위해 기존 수보다 1 작으므로 이 때는 1을 더한다.
							    // 이 때는 node가 0부터 시작하므로 1을 더한다.
        for (int i = 0; i <= node; i++)
            for (int j = 0; j <= node; j++)
                weightMatrix[i, j] = 999;   // 처음 weight의 행렬의 각 요소를 모두 999로 잡는다.(무한대로표현)
      	for (int i = 0; i <= node; i++)// 같은 node로 향하는 가중치는 0으로 둔다.
            weightMatrix[i, i] = 0;
    }
	
    // Update is called once per frame
    void Update () {
        if (floydWork) {                           		 // Floyd Algorithm 버튼을 누른 경우
       	    floydAlgorithm();                                		 // Floyd Algorithm 함수 실행
       	    outputPath(startPoint+1, endPoint+1);                  	 // Floyd 최단경로를 출력하는 함수 실행
	    floydWork = false;                                      // 종료
	}
	if (dijkstraWork) {                                        	 // Dijkstra Algorithm 버튼을 누른 경우
	    dijkstraAlgorithm (startPoint + 1, node + 1);            // Dijkstra Algorithm 함수 실행
	    outputTouch (startPoint + 1, endPoint + 1);              // Dijkstra 최단경로를 출력하는 함수 실행
	    dijkstraWork = false;                                    // 종료
	}
    }

    void changeEdgeColor(int startP, int endP){                     // 최단경로를 표시한 Edge의 색을 바꿔주는 함수
	if (startP == 1)
	    edgeImage0 [endP - 1].gameObject.GetComponent<Image>().color = Color.red;
	else if (startP == 2)
	    edgeImage1 [endP - 1].gameObject.GetComponent<Image>().color = Color.red;
	else if (startP == 3)
	    edgeImage2 [endP - 1].gameObject.GetComponent<Image>().color = Color.red;
	else if (startP == 4)
	    edgeImage3 [endP - 1].gameObject.GetComponent<Image>().color = Color.red;
	else if (startP == 5)
	    edgeImage4 [endP - 1].gameObject.GetComponent<Image>().color = Color.red;
    }

    void outputPath(int startP, int endP){
	if (Path [startP, endP] == 0) {                             // Path가 0을 가리키고 있는 것은 거쳐서 가는 경로가 없다는 뜻이므로 그 때의 edge를 출력한다.
	    changeEdgeColor (startP,endP);
	    Debug.Log ("v" + (endP));                           // 경로가 잘 표시되는 지
	} else {
            outputPath (startP, Path [startP, endP]);               // Path가 0을 가리키고 있지 않다면 거쳐서 가는 경로가 존재하므로 output함수를 재귀적으로 호출하며
            outputPath(Path[startP, endP],endP);                    // 출발점과 도착점 사이에 있는 요소를 중심으로 두 부분으로 나누어 재귀호출을 진행한다.
        }
    }
    void outputTouch(int startP, int endP){	
	if (touch [endP] != 0) {
	    outputTouch (endP,touch [endP]); // touch가 0을 가리키고 있지 않다면 거쳐서 가는 경로가 존재하므로 output함수를 재귀적으로 호출한다.
	    changeEdgeColor (touch[endP],endP); // endPoint에서 거꾸로 경로를 추적하므로 최단경로를 출력할 때는 변수를 반대로 넣어 불러온다.
	    Debug.Log ("v" + endP);
	} else {                       		      // 최단경로 추적이 끝나면 마지막 endP는 출발점 바로 다음에 오는 node가 된다. 따라서
	    changeEdgeColor (startPoint + 1,endP);// 출발점과 그 다음 나오는 node를 연결하는 edge 출력.
	    Debug.Log ("v" + endP);	
	}
    }

    void floydAlgorithm(){
	Path = new int[node + 2, node + 2];                  // 최단경로로 가는 node를 갖는 행렬이며 Path의 각 요소는 node가 갖고 있는 숫자와 관련되어 있다.
                                                                     // 하지만 Path에서 표현하려는 node를 0부터 시작하면 최단경로를 만들지 않는 node간의 edge도 0으로 표현할 것이기 때문에
                                                                     // 둘이 겹칠 가능성이 있으므로 Path는 정확하게 node수를 가리키기 위해 실제 node수 보다 하나 더 크게 만든다.
                                                                     // 그러나 전역 변수에서 지정한 node는 배열의 용이성을 위해 실제 node수 보다 1 작으므로 여기서는 + 2로 표현한다.
	D = new int[node + 1, node + 1];
	for (int i = 0; i <= node + 1; i++)
	    for (int j = 0; j <= node + 1; j++)
 	        Path [i,j] = 0;
		
		
		
	D = weightMatrix;                                           // D는 Floyd Algorithm에 사용되는 최단경로의 길이가 포함된 행렬이 되므로 가중치 행렬을 먼저 대입한다.
       	for (int k = 0; k <= node; k++)
	    for (int i = 0; i <= node; i++)
	        for (int j = 0; j <= node; j++) {
		    if (D [i, k] + D [k, j] < D [i, j]) {       // node vi, vk, vj가 있을 때 vi에서 vj로 바로 가는 것이 빠른지 중간에 vk를 거쳐서 가는 것이 빠른지 비교한다.
                        Path[i + 1, j + 1] = k + 1;                 // 만약 vk를 거쳐서 가는것이 가중치가 더 적을경우 그 때의 k를 Path[i,j]에 포함시킨다. 처음에 Path는 실제 node의 수로
		        D [i, j] = D [i, k] + D [k, j];     // 이루어진 행렬이기 때문에 path[i + 1, j + 1] = k + 1; 로 표현하는 것이다. 또한 최단거리를 나타내는 D도 k를 거쳐가는 갱신시킨다.
		    }                                           // 이 과정을 node의 수 만큼 반복하면 Path에는 최단경로로 가는 node를 갖는 요소들로 이루어진 행렬이 된다.
	        }
					
    }
    void dijkstraAlgorithm(int startP, int num){
	int count = 1; 						// while문 반복횟수
	int vnear = 1; 						// 각 node에 대해서 가장 가까이에 있는 vnear값
	touch = new int[num + 1]; 				// 배열은 0부터 시작하므로 (node + 1) 만큼 배열을 만들어 node 표시를 용이하게한다.
												    		                          // touch는 어떤 node에서 가장 가까운 node를 뜻한다.
	length = new int[num + 1];				// 마찬가지로 (node의 수 + 1) 만큼 배열을 만든다. 
												    		                          //length는 startPoint에서 다른 node로 가는 거리가 된다.
	for (int i = 1; i <= num; i++) {
	    touch [i] = startP;				// startPoint에서 가장 가까운 node를 일단 startPoint로 초기화 한다.
	    length [i] = weightMatrix [startP - 1, i - 1];  // weightMatrix는 0부터 시작하고 startPoint와 잇는 edge들의 weight 초기값을 length에 대입한다.
	}
	length [startP] = 0;                                    // startPoint는 weight의 최소값에서 제외시키기 위해 -1를 대입한다.
	while (node >= count) { 				// while문을 (node의 수 -1) 만큼 반복
	    int min = 999; 					// 먼저 infinite로 갱신
	    for(int i = 1; i <= num; i++){                  // for문을 (node의 수 -1) 만큼 반복
	        if (length [i] >= 1 && length [i] < min) {    // startPoint에서 그 다음 node로 가는 weight의 최소를 검사한다.
		    min = length [i];                     // weight의 최소를 갱신
		    vnear = i;                            // 그 때의 node Vi를 vnear에 저장한다.
	        }
	    }
	    for (int i = 1; i <= num; i++) {
	        if (length [vnear] + weightMatrix [vnear - 1, i - 1] < length [i]) { // startPoint에서 다른 node Vi로 갈 때 
																			         // 가장 weight가 적은 node를 거쳐가는 것이 더 이득인지 검사.
		    length [i] = length [vnear] + weightMatrix [vnear - 1, i - 1];// 그럴 경우 length를 갱신시킴으로써 더 짧은경로를 대입한다.
		    touch[i] = vnear; 										         // startPoint에서 Vi로 갈 때 Vnear을 거치는 것이 더 빠르므로 갱신한다.
	      }
 	    }
	    length [vnear] = 0;                                                     // startPoint에서 가장 가까운 Vnear로 갈 때 최소의 weight를 검사하는 과정에서 제외시키기 위함.
	    count++;
	 }
	 for (int i = 1; i <= node + 1; i++) {
	     if (touch [i] == startP) {
	         touch [i] = 0;
	     }
				
	     Debug.Log ("t = " + touch [i]);
	 }
     }  
       /*---------------------------------여기서는 UI 순서대로 정렬되어 있는 함수들----------------------------------------*/
    public void firstEdgeUpBtn(){
        if (selectNode1 == node) { // if more vedge, stop
        } else {
	    selectNode1++;
	    selectNode1Text.text = "v" + (selectNode1 + 1);
      }
    } 
    public void firstEdgeDownBtn(){
	if (selectNode1 == 0) { // if less v1, stop
	} else {
	    selectNode1--;
	    selectNode1Text.text = "v" + (selectNode1 + 1);
	}

    }
    public void secondEdgeUpBtn(){
	if (selectNode2 == node) { // if more vedge, stop
	} else {
	    selectNode2++;
	    selectNode2Text.text = "v" + (selectNode2 + 1);
	}
    }
    public void secondEdgeDownBtn(){
	if (selectNode2 == 0) { // if less v1, stop
	} else {
	    selectNode2--;
	    selectNode2Text.text = "v" + (selectNode2 + 1);
	}
    }
    public void weightPlusBtn(){
	if (weight == 9) { // decision weight
	} else {
	    weight++;
	    weightText.text = "" + (weight);
	}
    }
    public void weightMiusBtn(){
	if (weight == 1) { // decision weight
	} else {
	    weight--;
	    weightText.text = "" + (weight);
	}
    }
    public void addWeightBtn(){ 					// 노드 간 edge의 가중치를 추가하는 버튼을 클릭했을 때
	if(selectNode1 != selectNode2){				// 가중치를 부여하고싶은 두 노드가 같지 않다면
	    weightMatrix[selectNode1,selectNode2] = weight; // edge에 지정된 가중치를 부여한다.
	    if (selectNode1 == 0){
		edgeImage0 [selectNode2].gameObject.SetActive (true);
		edgeWeightText = edgeImage0 [selectNode2].GetComponentInChildren<Text> ();
		edgeWeightText.text = "" + weight;
            }
	    else if(selectNode1 == 1){
		edgeImage1 [selectNode2].gameObject.SetActive (true);
		edgeWeightText = edgeImage1 [selectNode2].GetComponentInChildren<Text> ();
		edgeWeightText.text = "" + weight;
	    }
	    else if(selectNode1 == 2){
		edgeImage2 [selectNode2].gameObject.SetActive (true);
		edgeWeightText = edgeImage2 [selectNode2].GetComponentInChildren<Text> ();
		edgeWeightText.text = "" + weight;
	    }
	    else if(selectNode1 == 3){
		edgeImage3 [selectNode2].gameObject.SetActive (true);
		edgeWeightText = edgeImage3 [selectNode2].GetComponentInChildren<Text> ();
		edgeWeightText.text = "" + weight;
	    }
	    else if(selectNode1 == 4){
		edgeImage4 [selectNode2].gameObject.SetActive (true);
		edgeWeightText = edgeImage4 [selectNode2].GetComponentInChildren<Text> ();
		edgeWeightText.text = "" + weight;
	    }
	    Debug.Log(""+weightMatrix[selectNode1,selectNode2]);
	}

    }
    public void startPointPlusBtn(){
	if (startPoint == node) { // if more vedge, stop
	} else {
       	    startPoint++;
	    startPointText.text = "v" + (startPoint + 1);
	}
    }
    public void startPointMinusBtn(){
	if (startPoint == 0) { // if less v1, stop
	} else {
	    startPoint--;
	    startPointText.text = "v" + (startPoint + 1);
	}

    }
    public void endPointPlusBtn(){
	if (endPoint == node) { // if more v5, stop
	} else {
	    endPoint++;
	    endPointText.text = "v" + (endPoint + 1);
	}
    }
    public void endPointMinusBtn(){
	if (endPoint == 0) { // if less v1, stop
	} else {
	    endPoint--;
	    endPointText.text = "v" + (endPoint + 1);
	}
    }
    public void floydstartBtn(){                    // floyd Button을 선택할 경우 1프레임마다 실행되는 update()에서 floyd를 동작한다.
	if (startPoint != endPoint) {	        // 출발점과 도착점은 같지 않을 때 실행된다
	    floydWork = true;
	}
    }
    public void dijkstrastartBtn(){                 // dijkstra Button을 선택할 경우 update()에서 dijkstra를 동작한다.
	if (startPoint != endPoint) {           // 출발점과 도착점은 같지 않을 때 실행된다
	    dijkstraWork = true;
	}
    }
}
