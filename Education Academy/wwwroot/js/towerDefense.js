let scene, camera, renderer, viruses = [];

window.initTowerDefense = (containerId) => {
    const container = document.getElementById(containerId);
    scene = new THREE.Scene();
    scene.background = new THREE.Color(0x050505); // فضاء مظلم

    camera = new THREE.PerspectiveCamera(75, container.clientWidth / container.clientHeight, 0.1, 1000);
    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(container.clientWidth, container.clientHeight);
    container.appendChild(renderer.domElement);

    // إضافة "السيرفر المركزي"
    const geometry = new THREE.BoxGeometry(2, 2, 2);
    const material = new THREE.MeshPhongMaterial({ color: 0x00ffff, emissive: 0x004444 });
    const server = new THREE.Mesh(geometry, material);
    scene.add(server);

    // إضاءة سينمائية
    const light = new THREE.PointLight(0xffffff, 1, 100);
    light.position.set(10, 10, 10);
    scene.add(light);

    camera.position.z = 15;
    animate();
};

window.fireLaser = () => {
    // منطق بصري لإطلاق شعاع ليزر وتفجير أقرب مكعب أحمر
    console.log("Laser Fired!");
};

function animate() {
    requestAnimationFrame(animate);
    renderer.render(scene, camera);
}